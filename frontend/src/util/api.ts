import config from "@/config";
import { IncomingMessage, OutgoingMessage } from "http";

export namespace Api {
  export interface NextIncomingMessage extends IncomingMessage {
    cookies: Record<string, string | undefined>;
  }
  export interface ResponseData<T> {
    data: T;
    headers: Headers;
  }
  export interface FetchOptions<T> {
    revalidate?: number;
    default?: T;
    headers?: Record<string, string | undefined>;
    req?: NextIncomingMessage;
    res?: OutgoingMessage;
    refreshOnUnauthorized?: boolean;
  }

  export async function post<T>(
    url: string | URL,
    body?: object,
    options: FetchOptions<T> = {}
  ) {
    return request<T>(url, "POST", JSON.stringify(body), {
      refreshOnUnauthorized: true,
      ...options,
      headers: { ...options.headers, "Content-Type": "application/json" },
    });
  }

  export async function get<T>(
    url: string | URL,
    options: FetchOptions<T> = {}
  ): Promise<ResponseData<T>> {
    return request<T>(url, "GET", undefined, {
      refreshOnUnauthorized: true,
      ...options,
    });
  }

  async function request<T>(
    url: string | URL,
    method: string,
    body?: string,
    options: FetchOptions<T> = {}
  ): Promise<ResponseData<T>> {
    const headers = options.headers ?? {};
    if (options.req) {
      headers["Cookie"] = options.req.headers.cookie;
    }

    let response: Response;
    try {
      response = await makeRequest(method, url, body, { ...options, headers });
    } catch (error) {
      if (options?.default !== undefined) {
        return {
          data: options.default,
          headers: new Headers(),
        };
      }

      throw error;
    }

    // For some reason NoContent responses don't have the Content-Length header.
    const hasBody =
      response.status !== 204 && response.headers.get("Content-Length") !== "0";

    const json = hasBody ? await response.json() : undefined;

    if (response.ok) {
      return {
        data: json as T,
        headers: response.headers,
      };
    }

    if (response.status === 401 && options?.refreshOnUnauthorized) {
      try {
        headers["Cookie"] = await refreshAccessToken(headers, options.res);

        // Retry the original request
        return request(url, method, body, {
          ...options,
          headers,
          refreshOnUnauthorized: false,
        });
      } catch (e) {
        console.debug("[User Session]: Access token refresh failed.", e);
      }
    }

    if (options?.default !== undefined) {
      return {
        data: options.default,
        headers: response.headers,
      };
    }

    if (json === undefined) {
      throw new Error(response.statusText);
    }

    throw new Error(JSON.stringify(json, undefined, 4));
  }

  async function makeRequest<T>(
    method: string,
    url: string | URL,
    body: string | undefined,
    options: FetchOptions<T> = {}
  ): Promise<Response> {
    const headers = options.headers ?? {};
    Object.keys(headers).forEach((key) => {
      if (headers[key] === undefined) {
        delete headers[key];
      }
    });

    try {
      return await fetch(url, {
        method,
        body,
        headers: headers as Record<string, string>,
        next: { revalidate: options?.revalidate },
        credentials: "include",
      });
    } catch (error) {
      console.error(error);
      throw error;
    }
  }

  export async function assertAccessTokenPresent(
    req: NextIncomingMessage,
    res: FetchOptions<unknown>["res"]
  ): Promise<void> {
    if (!req.cookies[config.cookies.accessToken]) {
      const headers = { Cookie: req.headers.cookie };
      req.headers.cookie = await refreshAccessToken(headers, res);
    }
  }

  async function refreshAccessToken(
    headers: Record<string, string | undefined>,
    res: FetchOptions<unknown>["res"]
  ): Promise<string> {
    console.debug("[User Session]: Access token expired. Refreshing...");

    const refreshTokenResponse = await post(
      `${config.api.baseUrl}/users/refresh`,
      undefined,
      {
        headers,
        refreshOnUnauthorized: false,
      }
    );

    // Forward the new cookies to the client
    res?.setHeader("Set-Cookie", refreshTokenResponse.headers.getSetCookie());

    console.log("[User Session]: Access token refreshed");

    const newCookies = (headers["Cookie"] = refreshTokenResponse.headers
      .getSetCookie()
      .map((cookie) => cookie.split(";")[0])
      .join("; "));

    return newCookies;
  }
}

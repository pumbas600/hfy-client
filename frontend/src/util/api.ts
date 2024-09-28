import config from "@/config";

export namespace Api {
  export interface FetchOptions<T> {
    revalidate?: number;
    default?: T;
    refreshOnUnauthorized?: boolean;
  }

  const IS_SERVER = typeof window === "undefined";

  export async function post<T>(
    url: string | URL,
    body?: object,
    options: FetchOptions<T> = {}
  ) {
    return request<T>(
      url,
      "POST",
      JSON.stringify(body),
      { "Content-Type": "application/json" },
      { refreshOnUnauthorized: true, ...options }
    );
  }

  export async function get<T>(
    url: string | URL,
    options: FetchOptions<T> = {}
  ) {
    return request<T>(url, "GET", undefined, undefined, {
      refreshOnUnauthorized: true,
      ...options,
    });
  }

  async function request<T>(
    url: string | URL,
    method: string,
    body?: string,
    headers: Record<string, string> = {},
    options?: FetchOptions<T>
  ): Promise<T> {
    let response: Response;

    try {
      response = await makeRequest(method, url, body, headers, options);
    } catch (error) {
      if (options?.default !== undefined) {
        return options.default;
      }

      throw error;
    }

    // For some reason NoContent responses don't have the Content-Length header.
    const hasBody =
      response.status !== 204 && response.headers.get("Content-Length") !== "0";

    const json = hasBody ? await response.json() : undefined;

    if (response.ok) {
      return json as T;
    }

    // if (response.status === 401 && options?.refreshOnUnauthorized) {
    //   try {
    //     const refreshTokenResponse = await makeRequest(
    //       "POST",
    //       `${config.api.baseUrl}/users/refresh`,
    //       undefined,
    //       headers,
    //       {
    //         refreshOnUnauthorized: false,
    //       }
    //     );

    //     if (refreshTokenResponse.ok) {
    //       // Update the cookies with the new access token.
    //       // IMPORTANT: This wont work if there are other cookies that need to be preserved.
    //       console.log(refreshTokenResponse.headers);
    //       headers["Cookie"] = refreshTokenResponse.headers
    //         .getSetCookie()
    //         .map((cookie) => cookie.split(";")[0])
    //         .join("; ");

    //       console.log(refreshTokenResponse.headers.getSetCookie());

    //       console.log("[User Session]: Access token refreshed");

    //       // Retry the original request
    //       return request(url, method, body, headers, {
    //         ...options,
    //         refreshOnUnauthorized: false,
    //       });
    //     } else {
    //       console.debug("[User Session]: Access token refresh failed.");
    //     }
    //   } catch (e) {
    //     console.debug("[User Session]: Access token refresh failed.", e);
    //   }
    // }

    if (options?.default !== undefined) {
      return options.default;
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
    headers: Record<string, string>,
    options: FetchOptions<T> = {}
  ): Promise<Response> {
    if (IS_SERVER) {
      // We can only import this in server-components.
      // const { headers: headersToForward } = await import("next/headers");
      // headers = {
      //   ...headers,
      //   ...Object.fromEntries(headersToForward().entries()),
      // };
    }

    try {
      return await fetch(url, {
        method,
        body,
        headers,
        next: { revalidate: options?.revalidate },
        credentials: "include",
      });
    } catch (error) {
      console.error(error);
      throw error;
    }
  }
}

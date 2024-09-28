export namespace Api {
  export interface ResponseData<T> {
    data: T;
    headers: Headers;
  }
  export interface FetchOptions<T> {
    revalidate?: number;
    default?: T;
    headers?: Record<string, string | undefined>;
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
    options?: FetchOptions<T>
  ): Promise<ResponseData<T>> {
    let response: Response;

    try {
      response = await makeRequest(method, url, body, options);
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
}

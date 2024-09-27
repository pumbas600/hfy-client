export namespace Api {
  export interface FetchOptions<T> {
    revalidate?: number;
    default?: T;
  }

  const IS_SERVER = typeof window === "undefined";

  export async function post<T>(
    url: string | URL,
    body: object,
    options?: FetchOptions<T>
  ) {
    return request<T>(
      url,
      "POST",
      JSON.stringify(body),
      { "Content-Type": "application/json" },
      options
    );
  }

  export async function get<T>(url: string | URL, options?: FetchOptions<T>) {
    return request<T>(url, "GET", undefined, undefined, options);
  }

  async function request<T>(
    url: string | URL,
    method: string,
    body?: string,
    headers: Record<string, string> = {},
    options?: FetchOptions<T>
  ): Promise<T> {
    let response: Response;
    if (IS_SERVER) {
      // We can only import this in server-components.
      const { cookies } = await import("next/headers");
      // Forward cookies from the server to the API.
      headers["Cookie"] = cookies().toString();
    }

    try {
      response = await fetch(url, {
        method,
        body,
        headers,
        next: { revalidate: options?.revalidate },
        credentials: "same-origin",
      });
    } catch (error) {
      console.error(error);

      if (options?.default !== undefined) {
        return options.default;
      }

      throw error;
    }

    const hasBody = response.headers.get("Content-Length") !== "0";

    const json = hasBody ? await response.json() : undefined;

    if (response.ok) {
      return json as T;
    }

    if (options?.default !== undefined) {
      return options.default;
    }

    if (json === undefined) {
      throw new Error(response.statusText);
    }

    throw new Error(JSON.stringify(json, undefined, 4));
  }
}

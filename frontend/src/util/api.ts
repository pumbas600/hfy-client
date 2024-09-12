export namespace Api {
  export interface FetchOptions<T> {
    revalidate?: number;
    default?: T;
  }

  export async function get<T>(
    url: string | URL,
    options?: FetchOptions<T>
  ): Promise<T> {
    let response: Response;
    try {
      response = await fetch(url, {
        next: { revalidate: options?.revalidate },
      });
    } catch (error) {
      console.error(error);

      if (options?.default !== undefined) {
        return options.default;
      }

      throw error;
    }

    const json = await response.json();

    if (response.ok) {
      return json as T;
    }

    if (options?.default !== undefined) {
      return options.default;
    }

    throw new Error(JSON.stringify(json, undefined, 4));
  }
}
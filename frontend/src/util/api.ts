export namespace Api {
  export interface FetchOptions {
    revalidate?: number;
  }

  export async function get<T>(
    url: string | URL,
    options?: FetchOptions
  ): Promise<T> {
    const response = await fetch(url, {
      next: { revalidate: options?.revalidate },
    });
    const json = await response.json();

    if (response.ok) {
      return json as T;
    }

    throw new Error(JSON.stringify(json, undefined, 4));
  }
}

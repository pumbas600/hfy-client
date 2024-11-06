import appConfig from "@/config";
import { MiddlewareConfig, NextRequest, NextResponse } from "next/server";
import serverConfig from "./config/serverConfig";

export function middleware(request: NextRequest): NextResponse {
  const isAuthenticated = request.cookies.has(appConfig.cookies.refreshToken);
  console.debug(`[Middleware]: User is authenticated: ${isAuthenticated}`);

  if (request.nextUrl.pathname === "/") {
    if (isAuthenticated) {
      return NextResponse.redirect(serverConfig.frontendUrl + "/r/HFY");
    }

    return NextResponse.next();
  }

  if (!isAuthenticated) {
    const loginUrl = new URL(serverConfig.frontendUrl + "/login");
    loginUrl.searchParams.set("return_url", request.nextUrl.pathname);
    return NextResponse.redirect(loginUrl);
  }

  return NextResponse.next();
}

export const config: MiddlewareConfig = {
  matcher: ["/", "/r/:path*", "/chapters/:path*", "/settings"],
};

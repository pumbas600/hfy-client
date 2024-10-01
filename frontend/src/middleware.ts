import appConfig from "@/config";
import { MiddlewareConfig, NextRequest, NextResponse } from "next/server";

export function middleware(request: NextRequest): NextResponse {
  const isAuthenticated = request.cookies.has(appConfig.cookies.refreshToken);
  console.debug(`[Middleware]: User is authenticated: ${isAuthenticated}`);

  if (!isAuthenticated) {
    return NextResponse.redirect(appConfig.fontendBaseUrl + "/login");
  }

  return NextResponse.next();
}

export const config: MiddlewareConfig = {
  matcher: ["/r/:path*", "/chapters/:path*", "/settings"],
};

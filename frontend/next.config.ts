import type { NextConfig } from "next";

function getApiOrigin() {
  const configuredUrl = process.env.NEXT_PUBLIC_API_URL?.trim();

  if (configuredUrl) {
    try {
      return new URL(configuredUrl).origin;
    } catch {
      return undefined;
    }
  }

  return process.env.NODE_ENV === "production" ? undefined : "http://localhost:5077";
}

const apiOrigin = getApiOrigin();
const connectSources = ["'self'", apiOrigin].filter(Boolean).join(" ");

const nextConfig: NextConfig = {
  async headers() {
    return [
      {
        source: "/(.*)",
        headers: [
          {
            key: "Content-Security-Policy-Report-Only",
            value:
              [
                "default-src 'self'",
                "base-uri 'self'",
                "object-src 'none'",
                "frame-ancestors 'none'",
                "form-action 'self'",
                "img-src 'self' data: blob:",
                "font-src 'self' data:",
                `connect-src ${connectSources}`,
                "style-src 'self' 'unsafe-inline'",
                "script-src 'self' 'unsafe-inline'",
                "worker-src 'self' blob:",
              ].join("; "),
          },
          { key: "Referrer-Policy", value: "strict-origin-when-cross-origin" },
          { key: "Permissions-Policy", value: "camera=(), microphone=(), geolocation=()" },
          { key: "X-Content-Type-Options", value: "nosniff" },
          { key: "X-Frame-Options", value: "DENY" },
        ],
      },
    ];
  },
};

export default nextConfig;

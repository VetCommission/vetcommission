export const endpoints = {
  health: "/health",
  auth: {
    login: "/api/auth/login",
    me: "/api/auth/me",
  },
  professionals: "/api/profissionais",
} as const;

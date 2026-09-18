export const accessResources = {
  appAccess: "app.access",
  adminDashboard: "admin.dashboard",
  professionalPortal: "professional.portal",
} as const;

export type AccessResource = (typeof accessResources)[keyof typeof accessResources];

export const allAccessResources = Object.values(accessResources);

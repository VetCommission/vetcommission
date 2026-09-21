export const professionalQueryKeys = {
  all: (tenantId: string) => ["tenant", tenantId, "professionals"] as const,
  list: (tenantId: string, filters: { search?: string; role?: string; active?: boolean }) => ["tenant", tenantId, "professionals", "list", filters] as const,
  detail: (tenantId: string, id: string) => ["tenant", tenantId, "professionals", "detail", id] as const,
};

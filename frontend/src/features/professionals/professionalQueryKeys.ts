export const professionalQueryKeys = {
  all: ["professionals"] as const,
  list: (filters: { search?: string; role?: string; active?: boolean }) => ["professionals", "list", filters] as const,
  detail: (id: string) => ["professionals", "detail", id] as const,
};

const currencyFormatter = new Intl.NumberFormat("pt-BR", {
  style: "currency",
  currency: "BRL",
});

const dateFormatter = new Intl.DateTimeFormat("pt-BR");

export function formatCurrency(value: number) {
  return currencyFormatter.format(value);
}

export function formatDate(value: Date | string) {
  return dateFormatter.format(typeof value === "string" ? new Date(value) : value);
}

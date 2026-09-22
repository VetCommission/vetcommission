import axios from "axios";

export type ApiErrorItem = { code: string; message: string; field?: string | null };
type ApiErrorEnvelope = { errors: ApiErrorItem[] };

const fallbackError: ApiErrorItem = {
  code: "Unexpected",
  message: "Não foi possível concluir a solicitação.",
};

export class ApiError extends Error {
  constructor(
    public readonly status: number | undefined,
    public readonly errors: ApiErrorItem[],
  ) {
    super(errors[0]?.message ?? fallbackError.message);
    this.name = "ApiError";
  }
}

export function getApiFieldErrors(error: unknown) {
  if (!(error instanceof ApiError)) return [];

  return error.errors
    .filter((item): item is ApiErrorItem & { field: string } => Boolean(item.field))
    .map((item) => ({
      field: item
        .field!.split(".")
        .map((part) => part.charAt(0).toLowerCase() + part.slice(1))
        .join("."),
      message: item.message,
    }));
}

export function getApiErrorMessage(error: unknown, fallback: string) {
  if (!(error instanceof ApiError)) return fallback;
  if (error.status === 401) return "Sua sessão expirou. Entre novamente para continuar.";
  if (error.status === 403) return "Você não possui permissão para realizar esta ação.";
  if (error.status === 409) return "Já existe um cadastro com esses dados.";
  if (error.status === 422 || error.errors.some((item) => item.code === "Validation")) {
    return error.errors.find((item) => !item.field)?.message ?? "Revise os dados informados.";
  }
  return fallback;
}

export function toApiError(error: unknown) {
  if (error instanceof ApiError) {
    return error;
  }

  if (axios.isAxiosError<ApiErrorEnvelope>(error)) {
    const errors = error.response?.data?.errors;
    return new ApiError(error.response?.status, errors?.length ? errors : [fallbackError]);
  }

  return new ApiError(undefined, [fallbackError]);
}

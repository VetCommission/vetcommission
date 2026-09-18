import axios from "axios";
import type { ApiErrorEnvelope, ApiErrorItem } from "@/types/api";

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

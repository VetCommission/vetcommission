export type ApiErrorItem = {
  code: string;
  message: string;
  field?: string | null;
};

export type ApiErrorEnvelope = {
  errors: ApiErrorItem[];
};

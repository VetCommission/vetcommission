"use client";

import { Button, Paper, Stack, Typography } from "@mui/material";
import Link from "next/link";
import type { FormEvent, ReactNode } from "react";

export type CrudFormMode = "create" | "view" | "edit";

type CrudFormShellProps = {
  title: string;
  mode: CrudFormMode;
  listHref: string;
  isSubmitting?: boolean;
  onSubmit: (event: FormEvent<HTMLFormElement>) => void;
  onEdit: () => void;
  onCancel?: () => void;
  onBeforeSubmit?: () => void;
  submitLabel?: string;
  editLabel?: string;
  canEdit?: boolean;
  children: ReactNode;
};

export function CrudFormShell({ title, mode, listHref, isSubmitting = false, onSubmit, onEdit, onCancel, onBeforeSubmit, submitLabel = "Salvar", editLabel = "Editar", canEdit = true, children }: CrudFormShellProps) {
  const readOnly = mode === "view";
  return <Paper variant="outlined" sx={{ width: "100%", p: { xs: 3, sm: 4, md: 5 } }}>
    <Stack component="form" spacing={3} onSubmit={event => { onBeforeSubmit?.(); onSubmit(event); }} noValidate>
      <Typography variant="h4">{title}</Typography>
      {children}
      <Stack direction="row" spacing={2} sx={{ justifyContent: "flex-end", pt: 2, borderTop: "1px solid", borderColor: "divider" }}>
        <Button component={Link} href={listHref} type="button" onClick={onCancel}>Voltar</Button>
        {readOnly ? (canEdit ? <Button type="button" variant="contained" onClick={event => { event.preventDefault(); event.stopPropagation(); onEdit(); }}>{editLabel}</Button> : null) : <Button type="submit" variant="contained" disabled={isSubmitting}>{submitLabel}</Button>}
      </Stack>
    </Stack>
  </Paper>;
}

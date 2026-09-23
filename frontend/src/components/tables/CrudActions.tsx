import { Stack } from "@mui/material";
import type { ReactNode } from "react";

export function CrudActions({ children, direction = "row" }: { children: ReactNode; direction?: "row" | "column" }) {
  return <Stack direction={direction} spacing={1} sx={{ width: "100%", marginLeft: "auto", justifyContent: "flex-end", alignItems: "flex-end", textAlign: "right", whiteSpace: "nowrap" }}>{children}</Stack>;
}

import { Container, type ContainerProps } from "@mui/material";
import type { ReactNode } from "react";

type CrudPageContainerProps = Omit<ContainerProps, "maxWidth" | "sx"> & {
  children: ReactNode;
};

export function CrudPageContainer({ children, ...props }: CrudPageContainerProps) {
  return <Container maxWidth="xl" sx={{ width: "100%", py: { xs: 3, md: 5 } }} {...props}>{children}</Container>;
}

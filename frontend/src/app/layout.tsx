import type { Metadata } from "next";
import "@fontsource/inter/400.css";
import "@fontsource/inter/500.css";
import "@fontsource/inter/600.css";
import "@fontsource/inter/700.css";
import { AppProviders } from "@/providers/AppProviders";
import "./globals.css";

export const metadata: Metadata = {
  title: "VetCommission",
  icons: {
    icon: "/assets/brand/favicon.svg",
    shortcut: "/assets/brand/favicon-32.png",
    apple: "/assets/brand/favicon-512.png",
  },
  description: "Gestão transparente de produção e comissões veterinárias.",
};

export default function RootLayout({ children }: LayoutProps<"/">) {
  return (
    <html lang="pt-BR">
      <body>
        <AppProviders>{children}</AppProviders>
      </body>
    </html>
  );
}

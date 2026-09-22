import { ProfessionalDetailsClient } from "@/features/professionals/ProfessionalDetailsClient";

export default async function ProfessionalDetailsPage({
  params,
}: {
  params: Promise<{ id: string }>;
}) {
  const { id } = await params;

  return <ProfessionalDetailsClient id={id} />;
}

"use client";

import config from "@/config";
import { Api } from "@/util/api";
import { Button } from "../atomic";
import { useRouter } from "next/navigation";

export default function LogoutButton() {
  const router = useRouter();

  const handleLogout = async (): Promise<void> => {
    try {
      await Api.post(`${config.api.baseUrl}/users/logout`);
      router.push("/");
    } catch (error) {
      console.log(error);
      // TODO: Display toast
    }
  };

  return (
    <Button variant="subtle" onClick={handleLogout}>
      Log out
    </Button>
  );
}

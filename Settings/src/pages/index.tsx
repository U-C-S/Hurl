import { useState } from "react";
import { invoke } from "@tauri-apps/api/tauri";
import Image from "next/image";
import reactLogo from "../assets/react.svg";
import tauriLogo from "../assets/tauri.svg";
import mantineLogo from "../assets/mantine.svg";
import {
  Button,
  Container,
  createStyles,
  Divider,
  Group,
  Stack,
  Text,
  TextInput,
  Title,
} from "@mantine/core";

const useStyles = createStyles((theme) => ({
  container: {
    paddingTop: "10vh",
  },

  NextInfo: {
    position: "absolute",
    bottom: "10px",
    width: "100%",
  },
}));

function App() {
  const [greetMsg, setGreetMsg] = useState("");
  const [name, setName] = useState("");
  const { classes } = useStyles();

  async function greet() {
    // Learn more about Tauri commands at https://tauri.app/v1/guides/features/command
    setGreetMsg(await invoke("greet", { name }));
  }

  return (
    <>
      <Container className={classes.container}>
        <Stack align="center" spacing={25}>
          <Title order={1}>Welcome !!!</Title>
          <Group position="center" spacing={50}>
            <span className="logos">
              <a href="https://mantine.dev" target="_blank">
                <Image
                  width={84}
                  height={84}
                  src={mantineLogo}
                  className="logo mantine"
                  alt="Mantine logo"
                />
              </a>
            </span>
            <span className="logos">
              <a href="https://tauri.app" target="_blank">
                <Image
                  width={84}
                  height={84}
                  src={tauriLogo}
                  className="logo tauri"
                  alt="Tauri logo"
                />
              </a>
            </span>
            <span className="logos">
              <a href="https://reactjs.org" target="_blank">
                <Image
                  width={84}
                  height={84}
                  src={reactLogo}
                  className="logo react"
                  alt="React logo"
                />
              </a>
            </span>
          </Group>
          <Text>
            Click on the Mantine, Tauri and React logos to learn more.
          </Text>

          <Divider w={500} />

          <Group position="center">
            <TextInput
              placeholder="Enter your name"
              onChange={(e) => setName(e.currentTarget.value)}
            />
            <Button onClick={() => greet()} variant="default">
              Greet
            </Button>
          </Group>

          <Text>{greetMsg}</Text>
        </Stack>
      </Container>
      <Text align="center" className={classes.NextInfo}>
        The frontend is powered with{" "}
        <a href="https://nextjs.org" target="_blank">
          Next.js
        </a>
      </Text>
    </>
  );
}

export default App;

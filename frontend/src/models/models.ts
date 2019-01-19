import { string, number } from "prop-types";

export interface Event {
    token: string;
    client_id: string;
    secret_key: string;
}
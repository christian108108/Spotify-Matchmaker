import * as React from "react";
import {Animated} from "react-animated-css";
export interface HelloProps { 
    greeting: string
}

// 'HelloProps' describes the shape of props.
// State is never set so we use the '{}' type.
export class Hello extends React.Component<HelloProps, {}> {
    render() {
        const WelcomeCSS:any = {
            fontSize: "35px",
            alignSelf: "center",
        };
        return <h1 style={WelcomeCSS}>{this.props.greeting}</h1>
    }
}
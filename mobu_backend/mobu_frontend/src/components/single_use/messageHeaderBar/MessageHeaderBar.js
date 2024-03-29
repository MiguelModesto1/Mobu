import React,{ useState, useEffect } from "react";
import TopTextBottomText from "../../modular/TopTextBottomText";
import ClickableIcon from "../../modular/ClickableIcon";
import "./MessageHeaderBar.css";
import OwnerOptionMenu from "../optionMenus/OwnerOptionMenu";
import Button from "../../modular/Button";

/**
 * 
 * Cabecalho do painel de mensagens
 * 
 * @param {*} text propriedades para o conjunto de textos
 * @returns 
 */
export default function MessageHeaderBar({owner, text, connections}){

    const [showMenu, setShowMenu] = useState(false);
    const [lastMessage, setLastMessage] = useState(text.bottom);
    const [anyChallenger, setAnyChallenger] = useState(false);

    useEffect(() => {
            connections[0].on("ReceiveMessage", function(user, message, idMsg){
            setLastMessage(message);
        })}, [connections]);
    
    const handleChevronClick = () => {
        setShowMenu(!showMenu);
    }

    const challenger = () =>{

        let fromUserVar;
        let usernameVar;

        connections[1].on("ReceiveChallenge", function(fromUser, username){
            fromUserVar = fromUser;
            usernameVar = username;
            setAnyChallenger(true);
        });

        return(anyChallenger? <div className="challenger-div">
            <TopTextBottomText TTBTProps={{top: usernameVar, bottom: fromUserVar}}/>
            <Button 
            text="Aceitar"
            color="#8ab9e5"
            fromParent="challenger-accept"
            onClick={() =>{
                connections[1].invoke("SendChallengeReply", owner, fromUserVar, true);
            }} />
            <Button 
            text="Recusar"
            color="#ff5f4a"
            fromParent="challenger-decline"
            onClick={() =>{
                connections[1].invoke("SendChallengeReply", owner, fromUserVar, false);
            }} />
        </div> : <></>);

    }

    return(
        <div className="message-header-bar">
            <TopTextBottomText TTBTProps={{top: text.top, bottom: lastMessage}}/>
            {challenger}
            <ClickableIcon 
            CIProps={{
                size:"48px",
                fill:"none",
                path:{
                    d:"M12 18L24 30L36 18",
                    stroke:"white",
                    strokeWidth:"10",
                    strokeLineCap:"round",
                    strokeLinejoin:"round"
                }
            }}
            onClick={handleChevronClick} />
            <OwnerOptionMenu showMenu={showMenu ? "none" : "block"}/>
        </div>
    );

    
}
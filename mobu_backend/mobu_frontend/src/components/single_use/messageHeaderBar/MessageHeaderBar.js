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

    useEffect(() => {
            connections.on("ReceiveMessage", function(user, message){
            setLastMessage(message);
        })}, [connections]);
    
    const handleChevronClick = () => {
        setShowMenu(!showMenu);
    }

    return(
        <div className="message-header-bar">
            <TopTextBottomText TTBTProps={{top: text.top, bottom: lastMessage}}/>
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
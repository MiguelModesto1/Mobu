import React,{ useMemo, useRef, useState } from "react";
import ClickableIcon from "../../modular/ClickableIcon";
import "./MessageFooterBar.css";

/**
 * 
 * Rodape do painel de mensagens
 * 
 * 
 * @returns 
 */
export default function MessageFooterBar({ ownerId, friendGroupData, selectedFriendItem, selectedGroupItem, isFriends, connection }) {
    
    const [message, setMessage] = useState("");

    const roomId = useRef();

    useMemo(() => {
        roomId.current = friendGroupData.length !== 0 ?
            isFriends ?
                friendGroupData[selectedFriendItem].CommonRoomId
                :
                friendGroupData[selectedGroupItem].IDSala
            :
            0
    },[friendGroupData, isFriends, selectedFriendItem, selectedGroupItem])

    const handleClickSend = async () => {
        try {
            if (message !== "") {
                if (isFriends) {
                    await connection.invoke("SendMessageToRoom", selectedFriendItem + "", ownerId + "", roomId.current + "", message);
                } else {
                    await connection.invoke("SendMessageToRoom", selectedGroupItem + "", ownerId + "", roomId.current + "", message);
                }
                
            }
        } catch (err) {
            console.log(err);
        }
        
    }

    const handleMsgChange = (value) => {
        setMessage(value);
    }

    return(
        <div className="message-header-bar">
            <div className="footer-bar-input-div">
                <input
                    type="text"
                    value={message}
                    className="footer-bar-input"
                    onChange={e => handleMsgChange(e.target.value)}
                />
            </div>
            <ClickableIcon 
            CIProps={{
                size:"71px",
                fill:"none",
                path:{
                    d:"M24.0009 37.9999V23.9999M24.5839 38.1692L38.5409 42.8416C39.6347 43.2078 40.1816 43.3909 40.5188 43.2595C40.8117 43.1454 41.0339 42.9002 41.1187 42.5976C41.2164 42.2491 40.9806 41.7228 40.5089 40.6701L25.532 7.24465C25.0708 6.21525 24.8402 5.70055 24.5189 5.54106C24.2398 5.4025 23.912 5.40198 23.6325 5.53964C23.3107 5.69809 23.0784 6.21205 22.6139 7.23996L7.50487 40.672C7.02948 41.724 6.79179 42.2499 6.88845 42.5988C6.97239 42.9018 7.19393 43.1477 7.48657 43.2626C7.82355 43.395 8.37134 43.2131 9.46691 42.8495L23.572 38.1679C23.7598 38.1056 23.8537 38.0744 23.9497 38.0621C24.0349 38.0512 24.1212 38.0513 24.2064 38.0624C24.3024 38.075 24.3963 38.1064 24.5839 38.1692Z",
                    stroke:"#6c6c6c",
                    strokeWidth:"2",
                    strokeLineCap:"round",
                    strokeLinejoin:"round"
                }
            }}
            onClick={handleClickSend}
            fromParent="footer-bar" />
        </div>
    );

    
}
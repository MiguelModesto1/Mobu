import React,{ useRef, useMemo, useEffect } from "react";
import "./MessagePanel.css"

/**
 * 
 * Painel de mensagens
 * 
 * @returns 
 */
export default function MessagePanel({ownerId, friendGroupData, selectedFriendItem, selectedGroupItem, isFriends}){ 
    //debugger;
    const messages = useRef(
        [{
            IDMensagem: 0,
            IDRemetente: 0,
            ConteudoMsg: ""
        }]
    );
    
    useMemo(() => {
        messages.current = friendGroupData.length !== 0 ?
            isFriends ?
                friendGroupData[selectedFriendItem].Messages
                :
                friendGroupData[selectedGroupItem].Mensagens
            :
            [{
                IDMensagem: 0,
                IDRemetente: 0,
                ConteudoMsg: ""
            }]
    }, [friendGroupData, isFriends, selectedFriendItem, selectedGroupItem]);

    const containers =
        messages.current.map(
            message => {

                if (message.IDRemetente === ownerId) {

                    return (
                        <div key={message.IDMensagem} className="owner-container-div">
                            <p>{message.ConteudoMsg}</p>
                        </div>
                    );
                } else {
                    return (
                        <div key={message.IDMensagem} className="other-users-container-div">
                            <p>{message.ConteudoMsg}</p>
                        </div>
                    );
                }
            }
        );

    return(
        <div className="message-panel">
            {containers}
        </div>
    );

    
}
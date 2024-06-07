import React, { useRef, useMemo, useEffect } from "react";
import "./MessagePanel.css"
import Avatar from "../../modular/Avatar";

/**
 * 
 * Painel de mensagens
 * 
 * @returns 
 */
export default function MessagePanel({ ownerId, friendGroupData, selectedFriendItem, selectedGroupItem, isFriends }) {
    //debugger;
    const messages = useRef(
        [{
            IDMensagem: 0,
            IDRemetente: 0,
            NomeRemetente: "",
            URLImagemRemetente: "",
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
                NomeRemetente: "",
                URLImagemRemetente: "",
                ConteudoMsg: ""
            }]
    }, [friendGroupData, isFriends, selectedFriendItem, selectedGroupItem]);

    const containers =
        messages.current.map(
            message => {

                return (
                    <div
                        key={message.IDMensagem}
                        className={message.IDRemetente === ownerId ?
                            "owner-container-div" : "other-users-container-div"
                        }
                    >
                        <Avatar avatarProps={{
                            size: "40px",
                            src: message.URLImagemRemetente,
                            alt: message.NomeRemetente
                        }} />
                        <span>{message.NomeRemetente}</span>
                        <p>{message.ConteudoMsg}</p>
                    </div>
                );

            }
        );

    return (
        <div className="message-panel">
            {containers}
        </div>
    );


}
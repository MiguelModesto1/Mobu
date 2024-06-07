import React, { useState, useEffect, useRef, useMemo } from "react";
import TopTextBottomText from "../../modular/TopTextBottomText";
import ClickableIcon from "../../modular/ClickableIcon";
import OwnerOptionMenu from "../../optionMenus/OwnerOptionMenu";
import Avatar from "../../modular/Avatar";

/**
 * 
 * Cabecalho do painel de mensagens
 * 
 * @param {*} text propriedades para o conjunto de textos
 * @returns 
 */
export default function MessageHeaderBar({ owner, personGroupData, selectedFriendItem, selectedGroupItem, isFriends, connection, logoutCallback }) {

    const [showMenu, setShowMenu] = useState(false);

    const roomName = useRef();

    useMemo(() => {
        roomName.current = personGroupData.length !== 0 ?
            isFriends ?
                personGroupData[selectedFriendItem].FriendName
                :
                personGroupData[selectedGroupItem].NomeSala
            :
            ""
    }, [isFriends, personGroupData, selectedFriendItem, selectedGroupItem]);

    const lastMessage = useRef();

    useMemo(() => {
        lastMessage.current =
            isFriends ?
                personGroupData[selectedFriendItem]
                    .Messages[personGroupData[selectedFriendItem].Messages.length - 1]
                :
                personGroupData[selectedGroupItem]
                    .Mensagens[personGroupData[selectedGroupItem].Mensagens.length - 1]
    }, [isFriends, personGroupData, selectedFriendItem, selectedGroupItem]);

    /**
     * clique no icone de menu
     */
    const handleMenuIconClick = () => {
        setShowMenu(!showMenu);
    }

    return (
        <div className="message-header-bar">
            {personGroupData.length !== 0 &&
                <Avatar avatarProps={{
                    size: "40px",
                    src:
                        isFriends ?
                            personGroupData[selectedFriendItem].ImageURL
                            :
                            personGroupData[selectedGroupItem].ImageURL,
                    alt:
                        isFriends ?
                            personGroupData[selectedFriendItem].FriendName
                            :
                            personGroupData[selectedGroupItem].NomeSala
                }} />
            }
            <TopTextBottomText
                TTBTProps={{
                    top: roomName.current,
                    bottom: lastMessage.current !== undefined ? lastMessage.current.ConteudoMsg : "Sem mensagens"
                }}
            />
            <ClickableIcon
                CIProps={{
                    size: "48px",
                    fill: "none",
                    path: {
                        d: "M12 18L24 30L36 18",
                        stroke: "black",
                        strokeWidth: "10",
                        strokeLineCap: "round",
                        strokeLinejoin: "round"
                    }
                }}
                onIconClick={handleMenuIconClick} />
            <OwnerOptionMenu owner={owner} showMenu={showMenu ? "block" : "none"} connection={connection} logoutCallback={logoutCallback} />
        </div>
    );


}
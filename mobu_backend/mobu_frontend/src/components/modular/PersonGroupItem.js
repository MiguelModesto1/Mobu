import React,{ useEffect, useMemo, useRef, useState } from "react";
import Avatar from "./Avatar";
import TopTextBottomText from "./TopTextBottomText";
import GroupContextMenu from "../single_use/optionMenus/GroupContextMenu"
import FriendContextMenu from "../single_use/optionMenus/FriendContextMenu"

/**
 * 
 * Item de amigos ou grupos do utilizador
 * 
 * @returns 
 */
export default function PersonGroupItem({ friendGroupData, onItemClick, connection, isSelectedItem, isFriends }) {

    const itemId = useRef();

    useMemo(() => {
        itemId.current = friendGroupData.ItemId
    }, [friendGroupData.ItemId]);

    const lastMessage = useRef();

    useMemo(() => {
        lastMessage.current =
            isFriends ?
                friendGroupData.Messages[friendGroupData.Messages.length - 1]
                :
                friendGroupData.Mensagens[friendGroupData.Mensagens.length - 1]
    }, [friendGroupData, isFriends]);

    const roomId = useRef();

    useMemo(() => {
        roomId.current =
            isFriends ?
                friendGroupData.CommonRoomId
                :
                friendGroupData.IDSala
    }, [friendGroupData.CommonRoomId, friendGroupData.IDSala, isFriends]);

    return (
        <div className="person-group-item" onClick={async () => {

            await connection.invoke("RemoveConnection", roomId.current + "");
            onItemClick(itemId.current);
            await connection.invoke("AddConnection", roomId.current + "");
        }}>
            <TopTextBottomText
                itemId={itemId.current}
                isSelectedItem={isSelectedItem}
                TTBTProps={{
                    top: isFriends ? friendGroupData.FriendName : friendGroupData.NomeSala,
                    bottom: lastMessage.current !== undefined ? lastMessage.current.ConteudoMsg : "Sem mensagens"
                }}
                fromParent="preson-group-item"
            />
        </div>
    );

    
}
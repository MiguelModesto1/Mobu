import React,{ useEffect, useState, useRef } from "react";
import Avatar from "./Avatar";
import TopTextBottomText from "./TopTextBottomText";

/**
 * 
 * Item do membro do grupo
 *
 * @returns 
 */
export default function GroupMemberItem({ requester, avatar, personId, personName, isAdmin, isRequesterAdmin, connection, roomId }) {
    //debugger
    /**
     * expulsao de membros do grupo
     * @param {any} member
     */
    const handleMemberExpelling = async (toUser, roomId) => {
        await connection.invoke("ExpelFromGroup", toUser, roomId);
    }

    return(
        <div className="person-group-found-item-div">
            <Avatar avatarProps={{
                src:avatar,
                alt:"avatar de " + personName,
                size:"40px"
            }}/>
            <TopTextBottomText 
            TTBTProps={{
                top:"#" + personId,
                bottom:personName
                }} />
            {isAdmin ? <span>Admin</span> : <></>}
            {isRequesterAdmin && requester !== personId ? <button onClick={() => handleMemberExpelling(personId+"", roomId+"")}>Expulsar</button> : <></>}
        </div>
    );

}
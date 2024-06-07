import React,{ useMemo, useLayoutEffect, useState } from "react";

/**
 * 
 * Cabecalho de separador
 * 
 * @returns 
 */
export default function TabHeader({ text, onHeaderClick, personGroupData, selectedItem, connection, isFriends }) { 

    return (
        <div className="tab-header"
            
            onClick={async () => {
            if (!isFriends) {
                await connection.invoke("RemoveConnection", personGroupData.group[selectedItem.group].IDSala + "");
                await connection.invoke("AddConnection", personGroupData.friend[selectedItem.friend].CommonRoomId + "");
            } else {
                await connection.invoke("RemoveConnection", personGroupData.friend[selectedItem.friend].CommonRoomId + "");
                await connection.invoke("AddConnection", personGroupData.group[selectedItem.group].IDSala + "");
            }
            onHeaderClick();
        }}>
            <span>{text}</span>
        </div>
    );

    
}
import React, { useMemo, useLayoutEffect, useState } from "react";

/**
 * 
 * Cabecalho de separador
 * 
 * @returns 
 */
export default function TabHeader({ tabsNumber, text, onHeaderClick, personGroupData, selectedItem, connection, isFriends }) {

    const backgroundColor = () => {
        if (text === "Amigos") {
            return isFriends ? "#c4dcf2" : "#3b9af1";
        }
        else {
            return isFriends ? "#3b9af1" : "#c4dcf2";
        }
    }

    const isBold = () => {
        if (text === "Amigos") {
            return isFriends ? "bold" : "normal";
        }
        else {
            return isFriends ? "normal" : "bold";
        }
    }

    return (
        <div
            className={"text-center w-" + 100 / tabsNumber + "  h-100"}
            style={{
                cursor: "pointer",
                backgroundColor: backgroundColor(),
                fontWeight: isBold(),
                color: "white",
                paddingTop: "1rem",
                paddingBottom: "1rem"
            }}

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
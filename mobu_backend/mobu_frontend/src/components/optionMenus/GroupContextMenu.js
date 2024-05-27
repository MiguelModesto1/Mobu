import React, { useState } from "react";
import { useContextMenu } from "../../hooks/useContextMenu";
import MenuItem from "../modular/MenuItem";

/**
 * 
 * Menu de contexto do grupo
 * 
 * @returns
 */
export default function GroupContextMenu({ itemId, owner, id, connection }) {

    const handleClick = async (option) => {
        switch (option) {
            case "perfil":
                await connection.stop();
                window.location.assign("/group-profile?id=" + id + "&requester=" + owner);
                break;
            default:
                await connection.invoke("LeaveGroup", itemId + "", owner + "", id + "");
                break;
        }
    }

    const { xPos, yPos, showMenu } = useContextMenu();
    return (
        <>
            {showMenu ? (
                <div
                    className="menu-container"
                    style={{
                        top: yPos,
                        left: xPos,
                        position: "absolute"
                    }}
                >
                    <MenuItem text="Perfil de grupo" onClick={handleClick} onClickPrm="perfil" />
                    <MenuItem text="Sair do grupo" onClick={handleClick} onClickPrm="sair" />
                </div>
            ) :
                <></>
            }
        </>
    );


}
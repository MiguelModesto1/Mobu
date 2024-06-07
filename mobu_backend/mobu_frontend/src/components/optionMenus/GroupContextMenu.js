import React, { useState, useEffect } from "react";
import { useContextMenu } from "../../hooks/useContextMenu";
import MenuItem from "../modular/MenuItem";

/**
 * 
 * Menu de contexto do grupo
 * 
 * @returns
 */
export default function GroupContextMenu({ hasLeft, wasExpelled, owner, isOwnerAdmin, id, connection }) {

    const { xPos, yPos, showMenu, setShowMenu } = useContextMenu();

    const handleClick = async (option) => {
        switch (option) {
            case "perfil":
                await connection.stop();
                window.location.assign("/group-profile?id=" + id + "&requester=" + owner);
                break;
            default:
                await connection.invoke("LeaveGroup", owner + "", id + "");
                setShowMenu(false);
                break;
        }
    }

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
                    {!isOwnerAdmin && !hasLeft && !wasExpelled &&
                        <MenuItem text="Sair do grupo" onClick={handleClick} onClickPrm="sair" />
                    }
                    
                </div>
            ) :
                <></>
            }
        </>
    );


}
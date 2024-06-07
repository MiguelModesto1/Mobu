import React,{ useEffect, useState } from "react";
import { useContextMenu } from "../../hooks/useContextMenu";
import MenuItem from "../modular/MenuItem";

/**
 * 
 * Menu de contexto do amigo
 * 
 * @returns
 */
export default function FriendContextMenu({ itemId, isFriendOverBlocked, hasFriendOverBlockedMe, owner, onBlock, id, connection }) { 

    const { xPos, yPos, showMenu, setShowMenu } = useContextMenu();

    const handleClick = async (option) => {
        switch (option) {
            case "perfil":
                await connection.stop();
                window.location.assign("/person-profile?id=" + id + "&requester=" + owner);
                break;
            default:
                if (isFriendOverBlocked) {
                    await connection.invoke("Unblock", owner + "", id + "");
                    onBlock(itemId, false);
                }
                else {
                    await connection.invoke("Block", owner + "", id + "");
                    onBlock(itemId, true);
                }
                setShowMenu(false);
                break;
            ;
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
                    <MenuItem text="Perfil" onClick={handleClick} onClickPrm="perfil" />
                    {
                        !hasFriendOverBlockedMe &&
                            <MenuItem text={isFriendOverBlocked ? "Desbloquear" : "Bloquear"} onClick={handleClick} onClickPrm="block_mngmt" />    
                    }
                    
                    {/*<MenuItem text="Reportar" onClick={handleClick} onClickPrm="reportar" />*/}
            </div>
        ) : (
            <></>
        )}
        </>
    );
}
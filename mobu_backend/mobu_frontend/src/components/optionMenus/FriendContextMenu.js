import React,{ useState } from "react";
import { useContextMenu } from "../../hooks/useContextMenu";
import MenuItem from "../modular/MenuItem";

/**
 * 
 * Menu de contexto do amigo
 * 
 * @returns
 */
export default function FriendContextMenu({ owner, id, connection }) { 

    const handleClick = async (option) => {
        switch(option){
            case "perfil":
                await connection.stop();
                window.location.assign("/person-profile?id=" + id + "&requester=" + owner);
                break;
            default:
                await connection.invoke("Block", owner + "", id + "");
                break;
            ;
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
                    <MenuItem text="Perfil" onClick={handleClick} onClickPrm="perfil" />
                    <MenuItem text="Bloquear" onClick={handleClick} onClickPrm="bloquear" />
                    {/*<MenuItem text="Reportar" onClick={handleClick} onClickPrm="reportar" />*/}
            </div>
        ) : (
            <></>
        )}
        </>
    );

    
}
import React,{ useState } from "react";
import { useContextMenu } from "../../../hooks/useContextMenu";
import MenuItem from "../../modular/MenuItem";

export default function FriendContextMenu({showMenuOnRightClick}){ 

    const handleClick = (endpoint) => {
        switch(endpoint){
            case "perfil" : return "";
            case "jogar" : return "";
            case "bloquear" : return "";
            default : return "";
        }
    }

    const { xPos, yPos, showMenu } = useContextMenu();
    return (
        <>
        {showMenu && showMenuOnRightClick ? (
            <div
            className="menu-container"
            style={{
                top: yPos,
                left: xPos,
                position: "absolute"
            }}
            >
                    <MenuItem text="Perfil" onClick={handleClick} onClickPrm="perfil" />
                    <MenuItem text="Jogar" onClick={handleClick} onClickPrm="jogar" />
                    <MenuItem text="Bloquear" onClick={handleClick} onClickPrm="bloquear" />
                    <MenuItem text="Reportar" onClick={handleClick} onClickPrm="reportar" />
            </div>
        ) : (
            <></>
        )}
        </>
    );

    
}
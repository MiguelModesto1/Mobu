import React,{ useState } from "react";
import { useContextMenu } from "../../../hooks/useContextMenu";
import MenuItem from "../../modular/MenuItem";

export default function GroupContextMenu({showMenuOnRightClick}){ 

    const handleClick = (endpoint) => {
        switch(endpoint){
            case "perfil" : return "";
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
                    <MenuItem text="Perfil de grupo" onClick={handleClick} onClickPrm="perfil" />
                    <MenuItem text="Sair do grupo" onClick={handleClick} onClickPrm="sair" />
            </div>
        ) : (
            <></>
        )}
        </>
    );

    
}
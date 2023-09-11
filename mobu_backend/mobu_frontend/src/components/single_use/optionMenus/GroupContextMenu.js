import React,{ useState } from "react";
import { useContextMenu } from "../../../hooks/useContextMenu";
import MenuItem from "../../modular/MenuItem";

export default function GroupContextMenu(){ 

    const handleClick = (endpoint) => {
        switch(endpoint){
            case "perfil" : return "";
            case "jogar" : return "";
            default : return "";
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
                    <MenuItem text="Jogar contra..." onClick={handleClick} onClickPrm="jogar" />
                    <MenuItem text="Sair do grupo" onClick={handleClick} onClickPrm="sair" />
            </div>
        ) : (
            <></>
        )}
        </>
    );

    
}
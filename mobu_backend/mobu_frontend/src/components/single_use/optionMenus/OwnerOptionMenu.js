import React,{ useState } from "react";
import { useContextMenu } from "../../../hooks/useContextMenu"
import MenuItem from "../../modular/MenuItem";

export default function OwnerOptionMenu(){ 

    const handleClick = (endpoint) => {
        switch(endpoint){
            case "perfil" : return "";
            case "jogar" : return "";
            case "procurar" : return "";
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
                        <MenuItem text="Meu perfil" onClick={handleClick} onClickPrm="perfil" />
                        <MenuItem text="Jogar" onClick={handleClick} onClickPrm="jogar" />
                        <MenuItem text="Procurar pessoas" onClick={handleClick} onClickPrm="procurar" />
                        <MenuItem text="Terminar sessão" onClick={handleClick} onClickPrm="terminar" />
                </div>
            ) : (
                <></>
        )}
        </>
    );
}
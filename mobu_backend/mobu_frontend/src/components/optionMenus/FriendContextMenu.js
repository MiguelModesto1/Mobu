import React,{ useState } from "react";
import { useContextMenu } from "../../hooks/useContextMenu";
import MenuItem from "../modular/MenuItem";

export default function FriendContextMenu({owner, id, showMenuOnRightClick}){ 

    const handleClick = (endpoint) => {
        switch(endpoint){
            case "perfil" : window.location.assign("./person-profile?id=" + id + "&isOwner=" + false); break;
            case "bloquear" : 
                var options = {
                    method:"POST",
                    redirect:"follow",
                    body:JSON.stringify(
                        {id:id,
                        owner:owner}
                    )
                }

                fetch(process.env.REACT_APP_API_URL + "/block", options)
                .then(resp => {
                    if(resp.status === 404){
                        window.location.assign("./error-404");
                    }
                })

                break;
            ;
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
                    <MenuItem text="Bloquear" onClick={handleClick} onClickPrm="bloquear" />
                    <MenuItem text="Reportar" onClick={handleClick} onClickPrm="reportar" />
            </div>
        ) : (
            <></>
        )}
        </>
    );

    
}
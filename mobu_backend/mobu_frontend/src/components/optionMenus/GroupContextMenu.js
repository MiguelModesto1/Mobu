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
                    className="menu-container d-flex flex-column col-lg-2 col-5"
                    style={{
                        //maxWidth: "15%",
                        top: yPos,
                        left: xPos,
                        position: "absolute"
                    }}
                >

                    {!hasLeft && !wasExpelled ?
                        <>
                            <MenuItem text="Perfil de grupo" onClick={handleClick} onClickPrm="perfil" />
                            {!isOwnerAdmin && <MenuItem text="Sair do grupo" onClick={handleClick} onClickPrm="sair" />}
                        </>
                        :
                        <span>Saiu do grupo!</span>
                    }

                </div>
            ) :
                <></>
            }
        </>
    );


}
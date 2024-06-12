import React, { useEffect, useState, useRef } from "react";
import Avatar from "./Avatar";
import TopTextBottomText from "./TopTextBottomText";

/**
 * 
 * Item do membro do grupo
 *
 * @returns 
 */
export default function GroupMemberItem({ requester, avatar, personId, personName, isAdmin, isRequesterAdmin, connection, roomId }) {
    //debugger
    /**
     * expulsao de membros do grupo
     * @param {any} member
     */
    const handleMemberExpelling = async (toUser, roomId) => {
        await connection.invoke("ExpelFromGroup", toUser, roomId);
    }

    return (
        <div
            className={`d-flex justify-content-between py-2 px-3 rounded-4`}
            
            style={{ backgroundColor: "#8ab9e5" }}
        >
            <div className="d-lg-flex">
                <div style={{ marginTop: ".5rem" }}>
                    <Avatar avatarProps={{
                        src: avatar,
                        alt: "avatar de " + personName,
                        size: "2rem"
                    }}
                    />
                </div>

                <div style={{ marginLeft: ".5rem" }}>
                    <TopTextBottomText
                        TTBTProps={{
                            top: "#" + personId,
                            bottom: personName
                        }}
                    />
                </div>
            </div>

            {isAdmin ?
                <div className="my-auto mx-2">
                    <span>Admin</span>
                </div>
                :
                <></>
            }
            {isRequesterAdmin && requester !== personId ?
                <div className="ml-2 d-lg-flex justify-content-center">
                    <button
                        className="btn rounded-4"
                        style={{ backgroundColor: "#ff5f4a", color: "white" }}
                        onClick={() => handleMemberExpelling(personId + "", roomId + "")}
                    >
                        <strong>
                            X
                        </strong>
                    </button>
                </div>
                : <></>
            }
        </div>
    );

}
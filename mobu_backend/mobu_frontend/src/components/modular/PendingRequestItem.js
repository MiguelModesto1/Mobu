import React, { useEffect, useState, useRef } from "react";
import Avatar from "./Avatar";



/**
 * Item de pedidos pendentes
 */
export default function PendingRequestItem({ connection, ownerId, personId, name, avatar }) {

    const [acceptButtonText, setAcceptButtonText] = useState("");
    const [refuseButtonText, setRefuseButtonText] = useState("");
    const [isAcceptClicked, setIsAcceptClicked] = useState(false);
    const [isRefuseClicked, setIsRefuseClicked] = useState(false);

    useEffect(() => {
        setAcceptButtonText("Aceitar");
        setRefuseButtonText("Recusar");

        connection.on("ReceiveRequestReply", (replierObject, reply) => {
            if (reply) {
                replierObject.friendId === personId &&
                setAcceptButtonText("Aceite");
            }
            else {
                replierObject.friendId === personId &&
                setRefuseButtonText("Recusado");
            }
        });

    }, []);

    /**
     * parametro que avalia se o botao 'Aceitar' ja foi clicado
     */
    function handleIsAcceptClicked() {
        setIsAcceptClicked(true);
    }

    /**
     * parametro que avalia se o botao 'Recusar' ja foi clicado
     */
    function handleIsRefuseClicked() {
        setIsRefuseClicked(true);
    }

    /**
     * clique no botao de aceitar o pedido
     * @returns
     */
    async function handleAcceptRequestButtonClick() {

        await connection.invoke("SendRequestReply", ownerId + "", personId + "", true);
    }

    /**
     * clique no botao de aceitar o pedido
     * @returns
     */
    async function handleRefuseRequestButtonClick() {

        await connection.invoke("SendRequestReply", ownerId + "", personId + "", false);
    }

    return (
        <div className="person-group-found-item-div">
            <Avatar avatarProps={{
                size: "40px",
                src: avatar,
                alt: "avatar de " + personId
            }} />
            <span>{personId}</span>
            <br />
            <span>{name}</span>
            <button
                className="accept-request-button"
                disabled={isAcceptClicked}
                style={{ backgroundColor: "#3b9ae1", display: isRefuseClicked ? "none" : "block" }}
                onClick={() => {
                    handleAcceptRequestButtonClick();
                    if (!isAcceptClicked)
                        handleIsAcceptClicked();
                }}
            >
                {acceptButtonText}
            </button>
            <button
                className="refuse-request-button"
                disabled={isRefuseClicked}
                style={{ backgroundColor: "#ff5f4a", display: isAcceptClicked ? "none" : "block" }}
                onClick={() => {
                    handleRefuseRequestButtonClick();
                    if (!isRefuseClicked)
                        handleIsRefuseClicked();
                }}
            >
                {refuseButtonText}
            </button>
        </div>
    );
}
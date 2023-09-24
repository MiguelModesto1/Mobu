import React,{ useEffect, useState, useRef } from "react";
import Button from "./Button";
import { HubConnection as signalR } from "@microsoft/signalr";


/**
 * 
 * Mosaico de jogo generico
 * 
 * @param {*} isRegistered booleano que indica se o utilizador e registado 
 * @returns 
 */
export default function GameTile(){

    const queryStrings = new URLSearchParams(window.location.href);

    const connectionId = queryStrings.get("connectionId");

    const connection = new signalR.HubConnectionBuilder()
    .withUrl(process.env.REACT_APP_HUB_URL + "/GameHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

    const id = queryStrings.get("id");
    const imageSrc = queryStrings.get("imageSrc");
    const imageSize = queryStrings.get("imageSize");
    const gameName = queryStrings.get("gameName");
    const isRegistered = queryStrings.get("isRegistered");

    
    const random = useRef(Array(2).fill(0));

    const [buttonSet,setButtonSet] = useState(<></>);


    useEffect(() => {

        var options = {
            method:'GET',
            redirect:'follow'
        }

        fetch(process.env.REACT_APP_API_URL + "/get-random-user-id", options)
        .then(res => {
            if(res.status !== 404){
                return res.json();
            }else{
                window.location.assign("./error-404")
            }
        })
        .then(data => {
            random.current[0] = data.randomId
            random.current[1] = data.userName
        });

        function handleRandomButtonClick(){
            connection.invoke("SendChallengeToUser", id, random.current[0], random.current[1]);
        }
    
        function handleFriendsButtonClick(){
            window.location.assign("./opponent-choice")
        }

        if(isRegistered){
            setButtonSet(
                <>
                    <Button text="Jogo Aleatório" fromParent="game-tile" onClick={handleRandomButtonClick}/>
                    <Button text="Desafiar amigos" fromParent="game-tile" onClick={handleFriendsButtonClick}/>
                </>
            );
        }else{
            setButtonSet(<Button text="Jogar" fromParent="game-tile" onClick={handleRandomButtonClick}/>);
        }
    }, [connection, id, isRegistered]);

    return(
    <div className="game-tile">
        <div className="game-img-div">
            <img 
            className="game-img"
            src={imageSrc}
            height={imageSize}
            alt={gameName} />
        </div>
        <div className="game-span-div">
            <span className="game-span">{gameName}</span>
        </div>
        <br />
        {buttonSet}
    </div>
    );

}
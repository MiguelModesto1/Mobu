import React,{ useEffect, useState } from "react";
import Button from "./Button";


/**
 * 
 * Mosaico de jogo generico
 * 
 * @param {*} gameTileProps propriedades do mosaico de jogo
 * @param {*} isRegistered booleano que indica se o utilizador e registado 
 * @returns 
 */
export default function GameTile({gameTileProps, isRegistered}){

    const [buttonSet,setButtonSet] = useState(<></>);

    function handleRandomButtonClick(){
        alert("Implementar handleRandomButtonClick de GameTile");
    }

    function handleFriendsButtonClick(){
        alert("Implementar handleFriendsButtonClick de GameTile");
    }


    useEffect(() => {
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
    }, []);

    return(
    <div className="game-tile">
        <div className="game-img-div">
            <img 
            className="game-img"
            src={gameTileProps.image.src}
            height={gameTileProps.image.size} />
        </div>
        <div className="game-span-div">
            <span className="game-span">{gameTileProps.gameName}</span>
        </div>
        <br />
        {buttonSet}
    </div>
    );

}
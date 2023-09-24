import React,{ useState } from "react";
import Avatar from "./Avatar";

/**
 * 
 * Item para escolher o oponente de jogo
 * 
 * @param {*} avatar origem do avatar
 * @param {*} opponent nome do oponente 
 * @returns 
 */
export default function ChooseOpponentItem({owner, connection, avatar, opponent, opponentId, numOpponents, maxOpponents, onClick}){

    const [iconSpanColor, setIconSpanColor] = useState("#ffffff");
    const [spanBold, setSpanBold] = useState("");
    const [divColor, setDivColor] = useState("#8ab9e5");
    const [isChosen, setIsChosen] = useState(false);

    return(
        <div 
        style={{background: divColor}}
        className="choose-opponent-item"
        onClick={e => {
            e.stopPropagation();
            if(!isChosen){
                setIconSpanColor("#000000");
                setDivColor("#c4dcf2");
                setSpanBold("");
                setIsChosen(true);
                onClick(numOpponents - 1);
            }else{
                if(numOpponents !== maxOpponents){
                    setIconSpanColor("#ffffff");
                    setDivColor("#8ab9e5");
                    setSpanBold("bold");
                    setIsChosen(false);
                    onClick(numOpponents + 1);
                }else{
                    connection.invoke("SendChallengeToUser", opponentId + "", opponent);
                    
                    connection.on("ReceiveReply", function(replier, interested){
                        if(interested){
                            window.location.assign("./game")
                        }else{
                            window.location.assign("./messages")
                        }
                    })
                }
                
            }
        }}>
            <Avatar avatarProps={{
                src:avatar,
                alt:"foto de " + opponent,
                size:"40px"
            }} />
            <div className="opponent-name-div">
                <span 
                style={{color: iconSpanColor , fontWeight: spanBold}}
                className="opponent-name-span">
                    {opponent}
                </span>
            </div>
            <div className={"choice-tick-icon"}>
                <svg 
                xmlns="http://www.w3.org/2000/svg"
                width={30} 
                height={30} 
                viewBox="0 0 30 30"
                fill="none">
                    <path 
                    d="M25 7.5L11.25 21.25L5 15" 
                    stroke={iconSpanColor}
                    strokeWidth={5}
                    strokeLinecap="round"
                    strokeLinejoin="round"/>
                </ svg>
            </div>
        </div>
    );

}
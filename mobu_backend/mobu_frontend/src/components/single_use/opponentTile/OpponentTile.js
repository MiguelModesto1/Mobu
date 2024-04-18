import React,{useEffect, useRef, useState} from 'react';
import ChooseOpponentItem from '../../modular/ChooseOpponentItem';

/**
 * Mosaico para escolher oponente de jogo
 * @returns 
 */
export default function OpponentTile(){

    const queryStrings = new URLSearchParams(window.location.href);

    const id = queryStrings.get("id");
    const connection = queryStrings.get("connection");

    const friends = useRef([]);

    const [numOpponents, setNumOpponents] = useState(0);

    useEffect(() =>{
        let options = {
            method:'GET',
            redirect:"follow"
        }

        let queryParams = `?id=${id}`

        fetch(process.env.REACT_APP_API_URL + "/get-friends" + queryParams, options)
        .then(resp => {
            if(resp.status !== 404){
                return resp.json()
            }
        })
        .then(data =>{
            friends.current = data.friends;
        })
        .catch(err => console.error("Erro: ", err));
    })

    /*const mapFriends = friends.current.map((friend) =>{

        return(
            <ChooseOpponentItem
            owner={id}
            connection={connection}
            avatar={friend[2]}
            opponent={friend[1]}
            opponentId={friend[0]}
            numOpponents={numOpponents}
            maxOpponents={1}
            onClick={handleClick} />
        );

    });*/

    const handleClick = (value) => {
        setNumOpponents(value)
    }

    return(
        <div className='opponent-tile'>
            <div className="opponent-tile-scroll-div">
                {/*mapFriends*/}
            </div>   
        </div>
    );

}
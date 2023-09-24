// https://react.dev/learn/tutorial-tic-tac-toe

import React,{ useEffect, useState } from "react";
import PlayerItem from "../../modular/PlayerItem";
import {HubConnectionBuilder as signalR} from "@microsoft/signalr";
import "./Game.css"

/**
 * POR IMPLEMENTAR
 */

export default function Game() {
    const [history, setHistory] = useState([Array(9).fill(null)]);
    const [currentMove, setCurrentMove] = useState(0);
    const xIsNext = currentMove % 2 === 0;
    const currentSquares = history[currentMove];
    const [stateReceived, setStateReceived] = useState(false);

    const connectionId = new URLSearchParams(window.location.href).get("connectionId");
    const challenger = new URLSearchParams(window.location.href).get("challenger");
    const opponent = new URLSearchParams(window.location.href).get("opponent");

    const connection = new signalR.HubConnectionBuilder()
    .withUrl(process.env.REACT_APP_HUB_URL + `/GameHub?connection=${connectionId}`)
    .configureLogging(signalR.LogLevel.Information)
    .build()

    connection.on("ReceiveGameRoomState", function(gameRoomState){
      const nextHistory = [...history.slice(0, currentMove + 1), JSON.parse(gameRoomState)];
      setHistory(nextHistory);
      setCurrentMove(nextHistory.length - 1);
      setStateReceived(true);
    });

    useEffect(() => {});
  
    function handlePlay(nextSquares) {
      const nextHistory = [...history.slice(0, currentMove + 1), nextSquares];
      setHistory(nextHistory);
      setCurrentMove(nextHistory.length - 1);
      setStateReceived(false);
      connection.invoke("SendGameRoomStateToUser", xIsNext ? challenger : opponent , JSON.stringify(nextSquares));
    }
  
    /*function jumpTo(nextMove) {
      setCurrentMove(nextMove);
    }*/
  
    return (
      <div className="game">
        <PlayerItem PIProps={{
          image: 
          {
            src:"./assets/images/logo512.png",
            size:"50px",
            alt:"eee"
          },
          username:"test"
        }}/>
        <div className="game-board">
          <GameBoard challenger={challenger} opponent={opponent} stateReceived={stateReceived} xIsNext={xIsNext} squares={currentSquares} onPlay={handlePlay} />
        </div>
        <PlayerItem PIProps={{
          image: 
          {
            src:"./assets/images/logo512.png",
            size:"50px",
            alt:"eee"
          },
          username:"test"
        }}/>
      </div>
    );
  }
  
  function calculateWinner(squares) {
    const lines = [
      [0, 1, 2],
      [3, 4, 5],
      [6, 7, 8],
      [0, 3, 6],
      [1, 4, 7],
      [2, 5, 8],
      [0, 4, 8],
      [2, 4, 6],
    ];
    for (let i = 0; i < lines.length; i++) {
      const [a, b, c] = lines[i];
      if (squares[a] && squares[a] === squares[b] && squares[a] === squares[c]) {
        return squares[a];
      }
    }
    return null;
  }
  

function GameBoard({challenger, opponent, stateReceived, xIsNext, squares, onPlay }) {
    function handleClick(i) {
      if (calculateWinner(squares)) {

        let options = {
          method:'POST',
          redirect:"follow",
          body:JSON.stringify({
              challenger: challenger,
              opponent: opponent,
              winner: xIsNext ? opponent : challenger
          }),
          headers: {
              'Content-type': 'application/json; charset=UTF-8'
          }
        }

        fetch(process.env.REACT_APP_API_URL + "/store-game-results", options)
        .then(resp => {
          if(resp.status === 204){
            window.location.assign("./messages")
          }
        });

        return;
      }

      if(squares[i]){

        let options = {
          method:'POST',
          redirect:"follow",
          body:JSON.stringify({
              challenger: challenger,
              opponent: opponent,
              winner: "none"
          }),
          headers: {
              'Content-type': 'application/json; charset=UTF-8'
          }
        }

        fetch(process.env.REACT_APP_API_URL + "/store-game-results", options)
        .then(resp => {
          if(resp.status === 204){
            window.location.assign("./messages")
          }
        });

        return;
      }
      const nextSquares = squares.slice();
      if (xIsNext) {
        nextSquares[i] = 'X';
      } else {
        nextSquares[i] = 'O';
      }
      onPlay(nextSquares);
    }
  
    const winner = calculateWinner(squares);
    let status;
    if (winner) {
      status = 'Winner: ' + winner;
    } else {
      status = 'Next player: ' + (xIsNext ? 'X' : 'O');
    }
  
    return (
      <>
        <div className="status">{status}</div>
        <div className="board-row">
          <Square value={squares[0]} onSquareClick={stateReceived ? () => handleClick(0) : undefined} />
          <Square value={squares[1]} onSquareClick={stateReceived ? () => handleClick(1) : undefined} />
          <Square value={squares[2]} onSquareClick={stateReceived ? () => handleClick(2) : undefined} />
        </div>
        <div className="board-row">
          <Square value={squares[3]} onSquareClick={stateReceived ? () => handleClick(3) : undefined} />
          <Square value={squares[4]} onSquareClick={stateReceived ? () => handleClick(4) : undefined} />
          <Square value={squares[5]} onSquareClick={stateReceived ? () => handleClick(5) : undefined} />
        </div>
        <div className="board-row">
          <Square value={squares[6]} onSquareClick={stateReceived ? () => handleClick(6) : undefined} />
          <Square value={squares[7]} onSquareClick={stateReceived ? () => handleClick(7) : undefined} />
          <Square value={squares[8]} onSquareClick={stateReceived ? () => handleClick(8) : undefined} />
        </div>
      </>
    );
  }
  

function Square({ value, onSquareClick }) {
    return (
      <button className="square" onClick={onSquareClick}>
        {value}
      </button>
    );
  }
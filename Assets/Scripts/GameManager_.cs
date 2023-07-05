using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_ : MonoBehaviour
{

    public TextMeshProUGUI gameovertext;
    public GameObject panel;
    public TextMeshProUGUI presskeytext;
    public int level = 0;
    public float levelMul = 0.02f;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;
    public List<Image> images = new List<Image>();
    public Ghost_[] ghosts;
    public Pacman_ pacman;
    public Transform pellets;
    public int ghostMult { get; private set; }
    // Start is called before the first frame update
    public int score { get; private set; }
    public int lives { get; private set; }
    void Start()
    {   
        NewGame();
    }
    // Update is called once per frame
    private void Update()
    {
        if (this.lives <= 0 && Input.anyKeyDown){
            NewGame();
        }
    }
    private void NewGame()
    {
        gameovertext.enabled = false;
        panel.active = false;
        presskeytext.enabled = false;
        SetScore(0);
        scoreText.text = "0 points";
        foreach(Image im in images)
        {
            im.enabled = true;
        }
        SetLives(3);
        level += 1;
        levelText.text = "Level: " + level.ToString();
        Invoke(nameof(NewRound), 1.0f);
        //NewRound();
    }
    private void NewRound()
    {

        

        foreach (Transform pellet in this.pellets)
        { 
            pellet.gameObject.SetActive(true);
        }
        ResetState();
        

    }
    private void ResetState()
    {
        ResetGhostMult();
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].ResetState();

        }
        this.pacman.ResetState();
        
       // this.pacman.transform.position.Set(0,-3.5f,-2);
    }
    private void GameOver()
    {
        gameovertext.enabled = true;
        panel.active = true;
        presskeytext.enabled = true;
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(false);

        }
        this.pacman.gameObject.SetActive(false);

    }
    private void SetScore(int score)
    {
        this.score = score;
    }
    private void SetLives(int Lives)
    {
        this.lives = Lives;
    }

    public void GhostEaten(Ghost_ ghost)
    {
        int points = ghost.points * ghostMult;
        SetScore(this.score + ghost.points * this.ghostMult);
        this.ghostMult += 1;
    }
    public void PacmanEaten()
    {
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(false);

        }
        pacman.DeathSequence();
        SetLives(this.lives - 1);
        if (this.lives > 0)
        {
            images[lives].enabled = false;
            Invoke(nameof(ResetState), 3.0f);
        }
        else
        {
            images[lives].enabled = false;
            GameOver();
        }

    }
    public void PelletEaten(Pellet_ pellet)
    {
        pellet.gameObject.SetActive(false);
        SetScore(this.score + pellet.points);
        scoreText.text = this.score.ToString() + " points";
        if (!HasRemainingPellets())
        {

            this.pacman.gameObject.SetActive(false);
            level += 1;
            levelText.text = "Level: " + level.ToString();
            Invoke(nameof(NewRound), 3.0f);
        }

    }
    public void GiantPelletEaten(GiantPellet_ pellet)
    {
        for(int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].frightened.Disable();
            this.ghosts[i].frightened.Enable(pellet.duration);
        }
        PelletEaten(pellet);
        CancelInvoke();
        Invoke(nameof(ResetGhostMult), pellet.duration);
    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in this.pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }

        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(false);

        }
        return false;
    }
    private void ResetGhostMult()
    {
        this.ghostMult = 1;
    }
   
}

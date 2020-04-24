﻿using System;
using System.Collections.Generic;

class Moon
{
    public int Px
    {
        get;
        private set;
    }
    public int Py
    {
        get;
        private set;
    }
    public int Pz
    {
        get;
        private set;
    }

    int Vx;
    int Vy;
    int Vz;

    public Moon(List<int> inputPosns)
    {
        Px = inputPosns[0];
        Py = inputPosns[1];
        Pz = inputPosns[2];
        Vx = 0;
        Vy = 0;
        Vz = 0;
    }

    public void updateVx(bool increase)
    {
        Vx += (increase ? 1 : -1);
    }

    public void updateVy(bool increase)
    {
        Vy += (increase ? 1 : -1);
    }

    public void updateVz(bool increase)
    {
        Vz += (increase ? 1 : -1);
    }

    public void updatePosition()
    {
        Px += Vx;
        Py += Vy;
        Pz += Vz;
    }

    int calcPotentialEnergy()
    {
        // Ep is the sum of the absolute values of a moon's x, y, and z position coordinates
        return Math.Abs(Px) + Math.Abs(Py) + Math.Abs(Pz);
    }

    int calcKineticEnergy()
    {
        // Ek is the sum of the absolute values of a moon's velocity coordinates.
        return Math.Abs(Vx) + Math.Abs(Vy) + Math.Abs(Vz);
    }

    public int calcTotalEnergy()
    {
        return calcPotentialEnergy() * calcKineticEnergy();
    }
}
